<template>
    <div>
        <h1>Products</h1>

        <b-button variant="success" @click="$refs.creationModal.show()">Add New</b-button>
        
        <PagedQuery v-bind:fields="fields" endpoint="products" startingSortBy="displayName" startingSortDir="desc"
        @item-clicked="productClicked">
        </PagedQuery>

        <ProductCreationModal ref="creationModal" />
  </div>
</template>

<script>
import PagedQuery from '@/components/Query/PagedQuery.vue'
import ProductCreationModal from '@/components/Modals/ProductCreationModal.vue'

export default {
    name: 'Products',
    components: { PagedQuery,ProductCreationModal },
    data: function() {
        return { 
            editedProduct: {},
            fields: [
                {
                    key: "displayName",
                    label: "Display Name",
                    sortable: true
                },
                {
                    key: "recommendedPrice",
                    label: "Recommended Price",
                    sortable: true
                }
            ]
        };
    },
    methods: {
        productClicked(row) {
            this.$router.push('/products/' + row.id);
        }
    }
}
</script>

<style>
</style>