<template>
    <div>
        <b-modal :id="modalId" title="Create Accounting Group" hide-footer>
            <b-form>
                <b-form-group label="Display name:" label-for="displayName">
                    <b-form-input id="displayName" v-model="displayName" type="text" required />
                </b-form-group>

                <b-form-group label="Sale Strategy:">
                    <SsSelector v-model="saleStrategyId" v-bind:required="false" />
                </b-form-group>

                <b-button type="submit" @click="createAccountingGroup" v-bind:disabled="!saleStrategyId">Create Accounting Group</b-button>
            </b-form>
        </b-modal>
    </div>
</template>

<script>
import SsSelector from '@/components/Selection/SsSelector.vue';

export default {
    name: 'AccountingGroupCreationModal',
    components: {SsSelector},
    props: {
        modalId: String
    },
    data: function() {
        return {
            displayName: "",
            saleStrategyId: ""
        };
    },
    methods: {
        createAccountingGroup: function(evt) {
            evt.preventDefault();
            var c = this;

            var formData = {
                displayName: c.displayName,
                saleStrategyId: c.saleStrategyId || null
            };

            c.$api.post("accountingGroups", formData)
                .then(response => {
                    var operationId = response.headers["x-operation"];
                    var posId = response.data.id;

                    var checkIfExistsPeriodically = function() {
                        c.$api.get("operations/" + operationId)
                            .then(resp => {
                                if (resp.isFaulted) {
                                    c.$bvModal.msgBoxOk("Accounting group creation transaction failed");
                                    return;
                                }

                                if (resp.isCompleted)
                                    c.$emit("ag-created", posId);
                                else
                                    setTimeout(checkIfExistsPeriodically, 1000);
                            })
                            .catch(err => {
                                if (err.response.status == 404)
                                    c.$emit("ag-created", posId);
                                else
                                    setTimeout(checkIfExistsPeriodically, 1000)
                            });
                    };

                    checkIfExistsPeriodically();
                })
                .catch(e => c.$api.showError(e, "Accounting group creation failed"));
        }
    }
}
</script>