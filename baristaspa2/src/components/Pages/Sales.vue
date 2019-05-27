<template>
    <div>
      <h1 v-if="!ofPos">Sales</h1>
    
      <PagedQuery v-bind:fields="fields" endpoint="sales" startingSortBy="created" startingSortDir="desc" @item-clicked="saleClicked">
        <template slot="created" slot-scope="data">
          {{ new Date(data.item.created).toLocaleString() }}
        </template>

        <template slot="pointOfSaleId" slot-scope="data">
          <PosName v-bind:id="data.item.pointOfSaleId" />
        </template>

        <template slot="productId" slot-scope="data">
          <ProductName v-bind:id="data.item.productId" />
        </template>

        <template slot="accountingGroupId" slot-scope="data">
          <AgName v-bind:id="data.item.accountingGroupId" />
        </template>

        <template slot="userId" slot-scope="data">
          <UserName v-bind:id="data.item.userId" />
        </template>
      </PagedQuery>

      <SaleDetailsModal modalId="saleDetails" v-bind:values="selectedSale" />
  </div>
</template>

<script>

import PagedQuery from '@/components/Query/PagedQuery.vue'
import AgName from '@/components/Display/AgName.vue'
import PosName from '@/components/Display/PosName.vue'
import ProductName from '@/components/Display/ProductName.vue'
import UserName from '@/components/Display/UserName.vue'
import SaleDetailsModal from '@/components/Modals/SaleDetailsModal.vue'
import Api from "@/api.js"

export default {
  name: 'Sales',
  components: { PagedQuery, AgName, UserName, ProductName, PosName, SaleDetailsModal },
  props: {
    ofPos: String
  },
  methods: {
    saleClicked(row) {
      this.showSaleDetails(row.id);
    },
    showSaleDetails(saleId) {
      var c = this;

      Api.get("sales/" + saleId)
        .then(resp => {
          c.selectedSale = resp.data;
          c.$bvModal.show("saleDetails");
        });
    }
  },
  data: function() {  
    return {
      selectedSale: {},
      fields: [
        {
          key: "created",
          label: "Created",
          sortable: true
        },
        {
          key: "pointOfSaleId",
          label: "Point of Sale",
          sortable: true
        },
        {
          key: "productId",
          label: "Product",
          sortable: true
        },
        {
          key: "userId",
          label: "User",
          sortable: true,
        },
        {
          key: "accountingGroupId",
          label: "Accounting Group",
          sortable: true
        },
        {
          key: "cost",
          label: "Cost",
          sortable: true
        },
        {
          key: "state",
          label: "State"
        }
      ]
    };
  }  
}
</script>

<style>
</style>