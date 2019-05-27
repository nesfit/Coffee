<template>
    <b-form @submit="updatePointOfSale">
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

        <b-button @click="deletePointOfSale" variant="danger" class="mr-2">Delete</b-button>
        <b-button type="submit" variant="primary" v-bind:disabled="hasMissingInfo || isSaving">Save</b-button>                 
    </b-form>
</template>

<script>
import AgSelector from '@/components/Selection/AgSelector.vue';
import SsSelector from '@/components/Selection/SsSelector.vue';

export default {
    name: 'PointOfSaleDetails',
    components: {AgSelector,SsSelector},
    computed: {
        hasMissingInfo() {
            return (!this.displayName || !this.displayName.length)
                || (!this.accountingGroupId || !this.accountingGroupId.length);
        }
    },
    data: function() {
        return {
            isSaving: false,
            posId: "",
            saleStrategyId: "",
            displayName: "",
            accountingGroupId: "",
            features: ""
        };
    },
    mounted() {
        this.posId = this.$route.params.id;
        var c = this;

        c.$api.get("pointsOfSale/" + this.posId).then(resp => {
            c.displayName = resp.data.displayName;
            c.accountingGroupId = resp.data.parentAccountingGroupId;
            c.saleStrategyId = resp.data.saleStrategyId;
            c.features = resp.data.features.join(",");
        });
    },
    methods: {
        updatePointOfSale: function(evt) {
            evt.preventDefault();
            var c = this;
            this.isSaving = true;

            var formData = {
                displayName: c.displayName,
                parentAccountingGroupId: c.accountingGroupId,
                saleStrategyId: c.saleStrategyId || null,
                features: c.features.length == 0 ? [] : c.features.split(',')
            };

            c.$api.put("pointsOfSale/" + c.posId, formData)
                .then(() => {
                    c.isSaving = false;
                    c.$eventHub.$emit('pos-renamed', c.posId, c.displayName);
                })
                .catch(() => {
                    c.isSaving = false;
                    c.$bvModal.msgBoxOk("Point of sale update failed");
                });
        },

        deletePointOfSale: function(evt) {
            evt.preventDefault();
            var c = this;

            this.$bvModal.msgBoxConfirm("Are you sure you want to delete this point of sale?")
                .then(val => {
                    if (!val) return;
                    c.$api.delete("pointsOfSale/" + this.posId)
                    .then(() => {
                        c.$bvModal.msgBoxOk("Point of sale deleted.")
                        .then(() => c.$router.push('/pointsOfSale'));
                    })
                    .catch(() => c.$bvModal.msgBoxOk("Point of sale deletion failed."));
                });
        }
    }
}
</script>