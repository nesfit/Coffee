<template>
    <div>
      <h1>Points of Sale</h1>

      <b-button variant="success" v-b-modal.newPosModal>Create New</b-button>

      <b-form>
        <TextFilter label="Display Name" v-model="displayName" />
      </b-form>
        
      <PagedQuery v-bind:fields="fields" endpoint="pointsOfSale" startingSortBy="displayName" startingSortDir="desc"
        @item-clicked="posClicked" v-bind:additionalQueryParams="additionalParams">
      </PagedQuery>

      <PointOfSaleCreationModal modalId="newPosModal" @pos-created="navigateToPos" />
  </div>
</template>

<script>

import PagedQuery from '@/components/Query/PagedQuery.vue'
import PointOfSaleCreationModal from '@/components/Modals/PointOfSaleCreationModal.vue'
import TextFilter from '@/components/Query/TextFilter.vue'

export default {
  name: 'PointsOfSale',
  components: { PagedQuery, PointOfSaleCreationModal, TextFilter},
  computed: {
    additionalParams() {
      return { displayName: this.displayName.length ? this.displayName : null };
    }
  },
  data: function() {
    return {
      displayName: "",
      fields: [
        {
          key: "displayName",
          label: "Display Name",
          sortable: true
        },
        {
          key: "id",
          label: "ID",
          sortable: true
        }
      ]
    };
  },
  methods: {
    posClicked(row) {
      this.navigateToPos(row.id);
    },

    navigateToPos(posId) {
      this.$router.push({ path: `/pointOfSale/${posId}`});
    }
  }
}
</script>

<style>
</style>