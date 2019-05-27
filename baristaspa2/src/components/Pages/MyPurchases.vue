<template>
  <div>
    <h1>My purchases</h1>
    <PagedQuery v-bind:fields="fields" endpoint="sales/me" startingSortBy="created" startingSortDir="desc">
        <template slot="created" slot-scope="data">
            {{ new Date(data.item.created).toLocaleString() }}
        </template>

        <template slot="pointOfSaleId" slot-scope="data">
            <PosName v-bind:id="data.item.pointOfSaleId" />
        </template>

        <template slot="productId" slot-scope="data">
            <ProductName v-bind:id="data.item.productId" />
        </template>
    </PagedQuery>
  </div>
</template>

<script>

import PagedQuery from "@/components/Query/PagedQuery.vue"
import PosName from "@/components/Display/PosName.vue"
import ProductName from "@/components/Display/ProductName.vue"

export default {
  name: "MyPurchases",
  components: {PagedQuery, PosName, ProductName},
  data: function() {
      return {
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
                key: "cost",
                label: "Cost",
                sortable: true
            },
            { key: "state", label: "State" }
          ]
      }
  }
}
</script>