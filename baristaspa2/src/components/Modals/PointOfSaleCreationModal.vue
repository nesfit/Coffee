<template>
    <div>
        <b-modal :id="modalId" title="Create Point of Sale" hide-footer>
            <b-form>
                <b-form-group label="Display Name (required)" label-for="displayName">
                    <b-form-input id="displayName" v-model="displayName" type="text" required />
                </b-form-group>

                <b-form-group label="Features" label-for="features">
                    <b-form-input id="features" v-model="features" type="text" />
                </b-form-group>

                <b-form-group label="Sale Strategy">
                    <SsSelector v-model="saleStrategyId" v-bind:required="false" />
                </b-form-group>

                <b-form-group label="Parent Accounting Group (required)">
                   <AgSelector v-model="accountingGroupId" v-bind:required="true" />
                </b-form-group>

                <b-button type="submit" @click="createPointOfSale" v-bind:disabled="hasMissingInfo">Create Point of Sale</b-button>
            </b-form>
        </b-modal>
    </div>
</template>

<script>
import Api from '@/api.js';
import AgSelector from '@/components/Selection/AgSelector.vue';
import SsSelector from '@/components/Selection/SsSelector.vue';

export default {
    name: 'PointOfSaleCreationModal',
    components: {AgSelector,SsSelector},
    props: {
        modalId: String
    },
    data: function() {
        return {
            "displayName": "",
            "accountingGroupId": "",
            "saleStrategyId": "",
            "features": ""
        };
    },
    computed: {
        hasMissingInfo() {
            return !this.displayName || !this.accountingGroupId;
        }
    },
    methods: {
        createPointOfSale: function(evt) {
            evt.preventDefault();
            var c = this;

            var formData = {
                displayName: c.displayName,
                parentAccountingGroupId: c.accountingGroupId,
                saleStrategyId: c.saleStrategyId || null,
                features: c.features.length == 0 ? [] : c.features.split(',')
            };

            Api.post("pointsOfSale", formData)
                .then(response => {
                    var operationId = response.headers["x-operation"];
                    var posId = response.data.id;

                    var checkIfExistsPeriodically = function() {
                        Api.get("operations/" + operationId)
                            .then(resp => {
                                if (resp.isFaulted) {
                                    c.$bvModal.msgBoxOk("Point of sale creation transaction failed");
                                    return;
                                }

                                if (resp.isCompleted)
                                    c.$emit("pos-created", posId);
                                else
                                    setTimeout(checkIfExistsPeriodically, 1000);
                            })
                            .catch(err => {
                                if (err.response.status == 404)
                                    c.$emit("pos-created", posId);
                                else
                                    setTimeout(checkIfExistsPeriodically, 1000)
                            });
                    };

                    checkIfExistsPeriodically();
                })
                .catch(() => c.$bvModal.msgBoxOk("Point of sale creation failed"));
        }
    }
}
</script>