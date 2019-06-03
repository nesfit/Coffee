<template>
    <div>
        <b-modal :id="modalId" title="New Stock Item" hide-footer>
            <b-form>
                <b-form-group label="Display name">
                    <b-form-input v-model="displayName" required />
                </b-form-group>

                <b-button type="submit" @click="createStockItem" v-bind:disabled="!displayName">Create</b-button>
            </b-form>
        </b-modal>
    </div>
</template>

<script>
export default {
    name: 'StockItemCreationModal',
    props: {
        modalId: String,
        posId: String
    },
    data: function() {
        return {
            displayName: ""
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
        },

        createStockItem: function(evt) {
            evt.preventDefault();
            var c = this;            
           
            c.$api.post("stockItems", { pointOfSaleId: c.posId, displayName: c.displayName })
                .then(resp => {
                    c.$emit("stock-item-created", resp.data.id);
                    c.$bvModal.hide(c.modalId);
                })
                .catch(e => c.$api.showError(e, "Stock item creation failed"));
        }
    }
}
</script>