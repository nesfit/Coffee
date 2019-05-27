<template>
    <div>
        <h1>Accounting Groups</h1>
    
        <b-button variant="success" v-b-modal.agCreationModal>Create New</b-button>

        <PagedQuery v-bind:fields="fields" endpoint="accountingGroups" startingSortBy="displayName" startingSortDir="desc" @item-clicked="agClicked">
        </PagedQuery>

        <AccountingGroupCreationModal modalId="agCreationModal" @ag-created="gotoAccountingGroup" /> 
  </div>
</template>

<script>

import PagedQuery from '@/components/Query/PagedQuery.vue'
import AccountingGroupCreationModal from '@/components/Modals/AccountingGroupCreationModal.vue'

export default {
  name: 'AccountingGroups',
  components: { PagedQuery,AccountingGroupCreationModal },
  data: function() {
    return { 
      fields: [
        {
            key: "displayName",
            label: "Display Name",
            sortable: true
        }
      ]
    };
  },
  methods: {
        agClicked(item) {
            this.gotoAccountingGroup(item.id)
        },
        gotoAccountingGroup(id) {
            this.$router.push("/accountingGroups/" + id);
        }
  }
}

</script>