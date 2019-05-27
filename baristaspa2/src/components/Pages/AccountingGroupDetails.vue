<template>
    <b-form>
        <b-form-group label="Display name:" label-for="displayName">
            <b-form-input id="displayName" v-model="displayName" type="text" v-bind:required="false" />
        </b-form-group>

        <b-form-group label="Sale Strategy:">
            <SsSelector v-model="saleStrategyId" />
        </b-form-group>

        <b-button @click="deleteAccountingGroup" variant="danger" class="mr-2">Delete</b-button>
        <b-button type="submit" variant="primary" @click="updateAccountingGroup" v-bind:disabled="!displayName || isSaving">Save</b-button>
    </b-form>
</template>

<script>
import SsSelector from '@/components/Selection/SsSelector.vue';

export default {
    name: 'AccountingGroupDetails',
    components: {SsSelector},
    data: function() {
        return {
            agId: "",
            displayName: "",
            saleStrategyId: "",
            isSaving: false
        };
    },
    mounted() {
        this.agId = this.$route.params.id;
        var c = this;

        c.$api.get("accountingGroups/"+this.agId).then(resp => {
            c.displayName = resp.data.displayName;
            c.saleStrategyId = resp.data.saleStrategyId;
        });
    },
    methods: {
        updateAccountingGroup: function(evt) {
            evt.preventDefault();
            var c = this;

            var formData = {
                displayName: c.displayName,
                saleStrategyId: c.saleStrategyId || null
            };

            c.isSaving = true;

            c.$api.put("accountingGroups/" + c.agId, formData)
                .then(() => {
                    c.isSaving = false;
                    this.$eventHub.$emit('ag-renamed', c.agId, c.displayName);
                })
                .catch(() => c.$bvModal.msgBoxOk("Accounting group update failed"));
        },

        deleteAccountingGroup: function(evt) {
            evt.preventDefault();
            var c = this;

             this.$bvModal.msgBoxConfirm("Are you sure you want to delete this accounting group?")
                .then(val => {
                    if (!val) return;
                    c.$api.delete("accountingGroups/" + this.agId)
                    .then(() => {
                        c.$bvModal.msgBoxOk("Accounting group deleted.").
                        then(() => c.$router.push('/pointsOfSale'));
                    })
                    .catch(() => c.$bvModal.msgBoxOk("Accounting group deletion failed."));
                });
        }
    }
}
</script>