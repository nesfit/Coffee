<template>
    <div>
        <b-modal :id="modalId" title="New Offer" hide-footer>
            <b-form>
                <b-form-group label="Recommended Price">
                    <b-form-input v-model="recommendedPrice"/>
                </b-form-group>          

                <b-form-group label="Product">
                    <ProductSelector label="Product" v-model="productId" v-bind:required="true" />
                </b-form-group>

                <b-form-group label="Stock Item">
                    <StockItemSelector label="Stock Item" v-bind:posId="posId" v-model="stockItemId" v-bind:required="false" />
                </b-form-group>
                
                <b-form-group label="Valid Since">
                    <DateTimeInput v-model="validSince" v-bind:required="true" />
                </b-form-group>

                <b-form-group label="Valid Until">
                    <DateTimeInput v-model="validUntil" v-bind:required="false" />
                </b-form-group>

                <b-button type="submit" @click="createOffer" v-bind:disabled="!canSubmit">Create Offer</b-button>
            </b-form>
        </b-modal>
    </div>
</template>

<script>
import ProductSelector from "@/components/Selection/ProductSelector.vue";
import StockItemSelector from "@/components/Selection/StockItemSelector.vue";
import DateTimeInput from "@/components/DateTimeInput.vue"

export default {
    name: 'OfferCreationModal',
    components: {ProductSelector,StockItemSelector,DateTimeInput},
    props: {
        modalId: String,
        posId: String
    },
    data: function() {
        return {
            productId: "",
            recommendedPrice: "",
            stockItemId: "",
            validSince: new Date().toString(),
            validUntil: ""
        };
    },
    computed: {
        canSubmit: function() {
            return !!this.productId;
        }
    },
    mounted() {
        this.updateForm();
    },
    methods: {
        updateForm: function() {
            this.productId = "";
            this.recommendedPrice = "";
            this.stockItemId = "";
            this.validSince = new Date().toString();
            this.validUntil = "";
        },
        
        createOffer: function(evt) {
            evt.preventDefault();
            var c = this;            

            if (typeof(c.recommendedPrice) == typeof("") && c.recommendedPrice.length == 0)
                c.recommendedPrice = null;
            if (typeof(c.validUntil) == typeof("") && c.validUntil.length == 0)
                c.validUntil = null;    
            if (typeof(c.stockItemId) == typeof("") && c.stockItemId.length == 0)
                c.stockItemId = null;
                
            var formData = {
                productId: c.productId,
                recommendedPrice: c.recommendedPrice,
                stockItemId: c.stockItemId,
                validSince: c.validSince,
                validUntil: c.validUntil,
                pointOfSaleId: c.posId
            };

            c.$api.post("offers", formData)
                .then(resp => {
                    c.$emit("offer-created", resp.data.id);
                    c.$bvModal.hide(c.modalId);
                    c.updateForm();
                })
                .catch(() => c.$bvModal.msgBoxOk("Offer creation failed"));
        }
    }
}
</script>