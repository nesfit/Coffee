<template>
    <div>
        <b-modal :id="modalId" title="Stock Item Details" hide-footer>
            <b-form>
                <b-form-group label="ID" label-for="id">
                    <b-form-input disabled :value="form.id" />
                </b-form-group>

                <b-form-group label="Display name">
                    <b-form-input v-model="form.displayName" required />
                </b-form-group>

                <b-button type="submit" @click="editStockItem">Edit Stock Item</b-button>
                <b-button type="submit" @click="deleteStockItem" variant="danger">Delete Stock Item</b-button> 
            </b-form>
        </b-modal>
    </div>
</template>

<script>
export default {
    name: 'StockItemDetailsModal',
    props: {
        modalId: String,
        values: Object
    },
    data: function() {
        return {
            form: {
                id: "",
                displayName: "",
                pointOfSaleId: ""
            }
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
            if (!this.values)
                return;

            this.form.id = this.values.id;
            this.form.displayName = this.values.displayName;
            this.form.pointOfSaleId = this.values.pointOfSaleId;
        },

        deleteStockItem: function(evt) {
            evt.preventDefault();
            var c = this;            
            var stockItem = this.values.id;

            c.$api.delete("stockItems/" + stockItem)
                .then(() => {
                    c.$bvModal.msgBoxOk("Stock item deleted");
                    c.$bvModal.hide(c.values.modalId);
                })
                .catch(e => c.$api.showError(e, "Stock item deletion failed"));
        },

        editStockItem: function(evt) {
            evt.preventDefault();
            var c = this;
            var stockItem = this.values.id;

            c.$api.put("stockItems/" + stockItem, c.form)
                .then(() => c.$bvModal.msgBoxOk("Stock item updated"))
                .catch(e => c.$api.showError(e, "Stock item editation failed"));
        }
    }
}
</script>