<template>
    <div>
        <b-modal :id="modalId" title="New Manual Stock Operation" hide-footer>
            <b-form>
                <b-form-group label="Stock Item (required)">
                    <StockItemSelector v-bind:posId="posId" v-model="stockItemId" label="Stock Item" v-bind:required="true" />
                </b-form-group>

                <b-form-group label="Quantity (required)">
                    <b-form-input v-model="quantity" required />
                </b-form-group>

                <b-form-group label="Comment (required)">
                    <b-form-input v-model="comment" required />
                </b-form-group>

                <b-button type="submit" @click="createStockItem" v-bind:disabled="!stockItemId || !comment">Create</b-button>
            </b-form>
        </b-modal>
    </div>
</template>

<script>
import StockItemSelector from '@/components/Selection/StockItemSelector.vue';

export default {
    name: 'ManualStockOperationCreationModal',
    props: {
        modalId: String,
        posId: String
    },
    components: {StockItemSelector},
    data: function() {
        return {
            comment: "",
            stockItemId: "",
            quantity: 1
        };
    },
    mounted() {
        this.updateForm();
    },
    watch: {
        values() {
            this.updateForm();
        }
    },
    methods: {
        updateForm: function() {
            this.displayName = "";
            this.pointOfSaleId = this.posId;
        },

        createStockItem: function(evt) {
            evt.preventDefault();
            var c = this;            
           
            c.$api.post("manualStockOperations", {comment:c.comment,stockItemId:c.stockItemId,quantity:c.quantity})
                .then(resp => {
                    c.$emit("stock-operation-created", resp.data.id);
                    c.$bvModal.hide(c.modalId);
                })
                .catch(e => c.$api.showError(e, "Manual stock operation creation failed"));
        }
    }
}
</script>