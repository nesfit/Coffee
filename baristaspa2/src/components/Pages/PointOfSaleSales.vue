<template>
    <div>
        <PagedQuery v-bind:fields="fields" v-if="posId" v-bind:endpoint="'sales/pointOfSale/' + posId" startingSortBy="created" startingSortDir="desc"
            @item-clicked="saleDetailRequested">
            <template slot="created" slot-scope="data">
                {{ new Date(data.item.created).toLocaleString() }}
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

        <SaleDetailsModal modalId="posSaleDetail" v-bind:values="selectedSale" />
    </div>
</template>

<script>

import Api from '@/api.js'
import PagedQuery from '@/components/Query/PagedQuery.vue'
import AgName from '@/components/Display/AgName.vue'
import ProductName from '@/components/Display/ProductName.vue'
import UserName from '@/components/Display/UserName.vue'
import SaleDetailsModal from '@/components/Modals/SaleDetailsModal.vue'

export default {
  name: 'Sales',
  components: { PagedQuery, AgName, UserName, ProductName, SaleDetailsModal },
  methods: {
    saleDetailRequested(row) {
        this.showSaleDetail(row.id);
    },

    showSaleDetail(saleId) {
        var c = this;

        Api.get("sales/"+saleId)
            .then(resp => {
                c.selectedSale = resp.data;
                c.$bvModal.show("posSaleDetail");
            });
    }
  },
  data: function() {  
    return {
        posId: null,
        selectedSale: {},
        fields: [
            {
                key: "created",
                label: "Created",
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
  },
  mounted() {
    this.posId = this.$route.params.id;
  }
}
</script>

<style>
</style>