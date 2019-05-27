<template>
    <div>
        <b-modal :id="modalId" title="New Stock Item" hide-footer>
            <b-form>
                <b-form-group label="Display name">
                    <b-form-input v-model="form.displayName" required />
                </b-form-group>

                <b-button type="submit" @click="createStockItem">Create</b-button>
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
            form: {
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
            this.form.displayName = "";
            this.form.pointOfSaleId = this.posId;
        },

        createStockItem: function(evt) {
            evt.preventDefault();
            var c = this;            
           
            c.$api.post("stockItems", c.form)
                .then(resp => {
                    c.$emit("stock-item-created", resp.data.id);
                    c.$bvModal.hide(c.modalId);
                })
                .catch(() => c.$bvModal.msgBoxOk("Stock item creation failed"));
        }
    }
}
</script>