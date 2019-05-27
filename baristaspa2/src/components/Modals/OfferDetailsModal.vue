<template>
    <div>
        <b-modal :id="modalId" title="Offer Details" hide-footer>
            <b-form>
                <b-form-group label="ID" label-for="id">
                    <b-form-input disabled :value="id" />
                </b-form-group>

                <b-form-group label="Recommended Price">
                    <b-form-input v-model="recommendedPrice"/>
                </b-form-group>          

                <b-form-group label="Product">
                    <ProductSelector v-model="productId" label="Product" v-bind:required="true" />
                </b-form-group>

                <b-form-group label="Stock Item">
                    <StockItemSelector v-bind:posId="pointOfSaleId" v-model="stockItemId" label="Stock Item" v-bind:required="false" />
                </b-form-group>

                <b-form-group label="Valid Since">
                    <DateTimeInput v-model="validSince" v-bind:required="true" />
                </b-form-group>

                <b-form-group label="Valid Until">
                    <DateTimeInput v-model="validUntil" v-bind:required="false" />
                </b-form-group>

                <b-button type="submit" @click="editOffer">Edit Offer</b-button>
                <b-button type="submit" @click="deleteOffer" variant="danger">Delete Offer</b-button> 
            </b-form>
        </b-modal>
    </div>
</template>

<script>
import ProductSelector from "@/components/Selection/ProductSelector.vue";
import StockItemSelector from "@/components/Selection/StockItemSelector.vue";
import DateTimeInput from "@/components/DateTimeInput.vue";

export default {
    name: 'OfferDetailsModal',
    components: {ProductSelector,StockItemSelector,DateTimeInput},
    props: {
        modalId: String,
        values: Object
    },
    data: function() {
        return {
            id: "",
            productId: "",
            recommendedPrice: 0,
            pointOfSaleId: "",
            stockItemId: "",
            validSince: "",
            validUntil: ""
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

            this.id = this.values.id;
            this.pointOfSaleId = this.values.pointOfSaleId;
            this.productId = this.values.productId;
            this.recommendedPrice = this.values.recommendedPrice;
            this.stockItemId = this.values.stockItemId;
            this.validSince = this.values.validSince;
            this.validUntil = this.values.validUntil ? this.values.validUntil : "";
        },
        
        deleteOffer: function(evt) {
            evt.preventDefault();
            var c = this;            
            var offer = this.values.id;

            c.$api.delete("offers/" + offer)
                .then(() => {
                    c.$bvModal.msgBoxOk("Offer deleted");
                    c.$bvModal.hide(c.values.modalId);
                })
                .catch(() => c.$bvModal.msgBoxOk("Offer deletion failed"));
        },

        editOffer: function(evt) {
            evt.preventDefault();
            var c = this;
            var offer = this.values.id;

            if (typeof(c.recommendedPrice) == typeof("") && c.recommendedPrice.length == 0)
                c.recommendedPrice = null;

            if (typeof(c.validUntil) == typeof("") && c.validUntil.length == 0)
                c.validUntil = null;
                
            if (typeof(c.stockItemId) == typeof("") && c.stockItemId.length == 0)
                c.stockItemId = null;

            var formData = {
                productId: c.productId,
                recommendedPrice: c.recommendedPrice,
                pointOfSaleId: c.pointOfSaleId,
                stockItemId: c.stockItemId,
                validSince: c.validSince,
                validUntil: c.validUntil
            };

            c.$api.put("offers/" + offer, formData)
                .then(() => c.$bvModal.msgBoxOk("Offer updated"))
                .catch(() => c.$bvModal.msgBoxOk("Offer failed to update"));
        }
    }
}
</script>