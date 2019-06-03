<template>
    <div>
        <b-button variant="success" v-b-modal.posOfferCreation>Add New</b-button>

        <PagedQuery v-bind:fields="fields" endpoint="offers" v-bind:additionalQueryParams="queryParams" @item-clicked="offerClicked">
            <template slot="productId" slot-scope="data">
                <ProductName v-bind:id="data.item.productId" />
            </template>

            <template slot="stockItemId" slot-scope="data">
                <StockItemName v-if="data.item.stockItemId" v-bind:id="data.item.stockItemId" />
                <span v-else>&mdash;</span>
            </template>

            <template slot="validSince" slot-scope="data">
                <TimeAgo v-if="data.item.validSince" v-bind:moment="data.item.validSince" />
                <span v-else>&mdash;</span>
            </template>

            <template slot="validUntil" slot-scope="data">
                <TimeAgo v-if="data.item.validUntil" v-bind:moment="data.item.validUntil" />
                <span v-else>&mdash;</span>
            </template>
        </PagedQuery>

        <OfferDetailsModal modalId="posOfferDetail" v-bind:values="selectedOffer" />
        <OfferCreationModal modalId="posOfferCreation" v-bind:posId="posId" @offer-created="offerCreated" />
    </div>
</template>

<script>
import PagedQuery from '@/components/Query/PagedQuery.vue'
import ProductName from '@/components/Display/ProductName.vue'
import StockItemName from '@/components/Display/StockItemName.vue'
import OfferDetailsModal from '@/components/Modals/OfferDetailsModal.vue'
import OfferCreationModal from '@/components/Modals/OfferCreationModal.vue'
import TimeAgo from '@/components/Display/TimeAgo.vue'

export default {
  name: 'PointOfSaleOffers',
  components: { PagedQuery, StockItemName, ProductName, OfferDetailsModal, OfferCreationModal, TimeAgo },
  methods: {
      offerClicked(row) {
          this.showOfferDetails(row.id);
      },
      showOfferDetails(offerId) {
          var c = this;

          c.$api.get("offers/"+offerId)
            .then(resp => {
                c.selectedOffer = resp.data;
                c.$bvModal.show("posOfferDetail");
            });
      },
      offerCreated(id) {
          this.showOfferDetails(id);
      }
  },
  data: function() {
    return {
      posId: null,
      selectedOffer: {},
      queryParams: {},
      fields: [
        {
          key: "productId",
          label: "Product",
          sortable: true
        },
        {
          key: "recommendedPrice",
          label: "Recommended Price",
          sortable: true
        },
        {
          key: "stockItemId",
          label: "Stock Item",
          sortable: true,
        },
        {
          key: "validSince",
          label: "Valid Since",
          sortable: true
        },
        {
          key: "validUntil",
          label: "Valid Until",
          sortable: true
        }
      ]
    };
  },
  mounted() {
    this.posId = this.$route.params.id;
    this.queryParams = { atPointOfSaleId: this.posId };
  }
}
</script>