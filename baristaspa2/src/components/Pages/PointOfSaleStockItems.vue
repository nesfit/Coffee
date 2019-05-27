<template>
    <div v-if="posId">
      <b-button variant="success" v-b-modal.stockItemCreation>Add New</b-button>

      <PagedQuery v-bind:fields="fields" v-bind:endpoint="'stockItems/atPointOfSale/' + posId" @item-clicked="stockItemClicked">
        <template slot="balance" slot-scope="data">
            <StockItemBalance v-bind:id="data.item.id" />
        </template>
      </PagedQuery>

      <StockItemDetailsModal modalId="stockItemDetails" v-bind:values="selectedStockItem" />
      <StockItemCreationModal modalId="stockItemCreation" :posId="posId" @stock-item-created="showDetails" />
    </div>
</template>

<script>
import PagedQuery from '@/components/Query/PagedQuery.vue'
import StockItemDetailsModal from '@/components/Modals/StockItemDetailsModal.vue'
import StockItemCreationModal from '@/components/Modals/StockItemCreationModal.vue'
import StockItemBalance from '@/components/Display/StockItemBalance.vue'

export default {
  name: 'PointOfSaleStockItems',
  components: { StockItemBalance, PagedQuery, StockItemDetailsModal, StockItemCreationModal },
  methods: {
    stockItemClicked(row) {
      this.showDetails(row.id);
    },

    showDetails(stockItemId) {
      var c = this;

      c.$api.get("stockItems/"+stockItemId)
        .then(resp => {
          c.selectedStockItem = resp.data;
          c.$bvModal.show("stockItemDetails");
        });
    }
  },
  data: function() {
    return {
      posId: null,
      selectedStockItem: {},
      fields: [
        {
          key: "displayName",
          label: "Display name",
          sortable: true
        },
        {
          key: "balance",
          label: "Balance"
        }
      ]
    };
  },
  mounted() {
    this.posId = this.$route.params.id;
  }
}
</script>