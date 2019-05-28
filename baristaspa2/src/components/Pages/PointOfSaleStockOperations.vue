<template>
    <div v-if="posId">
      <b-tabs v-if="stockItemIdsLoaded">
        <b-tab title="Sale-based stock operations" active>
            <PagedQuery v-if="anyStockItems" @item-clicked="removeSaleBasedStockOp" v-bind:fields="saleBasedFields" endpoint="saleBasedStockOperations" v-bind:additionalQueryParams="queryParams" startingSortBy="created" startingSortDir="desc">
                <template slot="stockItemId" slot-scope="data">
                    <StockItemName v-if="data.item.stockItemId" v-bind:id="data.item.stockItemId" />
                </template>
            </PagedQuery>

            <div v-else>No stock items available at this point of sale.</div>
        </b-tab>

        <b-tab title="Manual stock operations">
            <b-button variant="success" v-b-modal.manualStockOpModal>Create manual stock operation</b-button>

            <PagedQuery v-if="anyStockItems" @item-clicked="removeManualStockOp" v-bind:fields="manualFields" endpoint="manualStockOperations" v-bind:additionalQueryParams="queryParams" startingSortBy="created" startingSortDir="desc">
                <template slot="stockItemId" slot-scope="data">
                    <StockItemName v-if="data.item.stockItemId" v-bind:id="data.item.stockItemId" />
                </template>

                <template slot="createdByUserId" slot-scope="data">
                    <UserName v-if="data.item.createdByUserId" v-bind:id="data.item.createdByUserId" />
                </template>
            </PagedQuery>

            <div v-else>No stock items available at this point of sale.</div>
        </b-tab>
      </b-tabs>

      <ManualStockOperationCreationModal v-bind:posId="posId" modalId="manualStockOpModal" />
    </div>
</template>

<script>
import PagedQuery from '@/components/Query/PagedQuery.vue'
import StockItemName from '@/components/Display/StockItemName.vue'
import UserName from '@/components/Display/UserName.vue'
import ManualStockOperationCreationModal from '@/components/Modals/ManualStockOperationCreationModal.vue'

export default {
  name: 'PointOfSaleStockOperations',
  components: { PagedQuery, StockItemName, UserName, ManualStockOperationCreationModal },
  computed: {
    queryParams: function() {
      return {stockItemId: this.stockItemIds};
    },
    anyStockItems: function() {
      return this.stockItemIds.length > 0;
    }
  },
  mounted() {
    this.posId = this.$route.params.id;
    this.reload();
  },
  methods: {
    reload() {
      if (!this.posId)
        return;

      var c = this;

      c.$api.get("stockItems/atPointOfSale/" + c.posId + "/?ResultsPerPage=100")
        .then(resp => {
          c.stockItemIds = [];
          for (var i = 0; i < resp.data.items.length; i++)
            c.stockItemIds.push(resp.data.items[i].id);

          c.stockItemIdsLoaded = true;
        });
    },

    removeSaleBasedStockOp(item) {
      var c = this;

      this.$bvModal.msgBoxConfirm("Are you sure you want to remove this sale-based stock operation?")
        .then(val => {
          if (!val) return;
          c.$api.delete("saleBasedStockOperations/" + item.id)
            .then(() => c.$bvModal.msgBoxOk("Sale-based stock operation removed."))
            .catch(() => c.$bvModal.msgBoxOk("Sale-based stock operation failed to remove."))
        });
    },

    removeManualStockOp(item) {
      var c = this;

      this.$bvModal.msgBoxConfirm("Are you sure you want to remove this manual stock operation?")
        .then(val => {
          if (!val) return;
          c.$api.delete("manualStockOperations/" + item.id)
            .then(() => c.$bvModal.msgBoxOk("Manual stock operation removed."))
            .catch(() => c.$bvModal.msgBoxOk("Manual stock operation failed to remove."))
        });
    }
  },
  data: function() {
    return {
      posId: null,
      stockItemIds: [],
      stockItemIdsLoaded: false,
      selectedOffer: {},
      saleBasedFields: [
        {
          key: "id",
          label: "ID",
          sortable: true
        },
        {
          key: "saleId",
          label: "Sale ID",
          sortable: true
        },
        {
          key: "stockItemId",
          label: "Stock Item",
          sortable: true,
        },
        {
          key: "quantity",
          label: "Quantity",
          sortable: true
        }
      ],
      manualFields: [
        {
          key: "id",
          label: "ID",
          sortable: true
        },
        {
          key: "stockItemId",
          label: "Stock Item",
          sortable: true,
        },
        {
          key: "quantity",
          label: "Quantity",
          sortable: true
        },
        {
          key: "createdByUser",
          label: "Created By"
        },
        {
          key: "comment",
          label: "Comment"
        }
      ]
    };
  }
}
</script>