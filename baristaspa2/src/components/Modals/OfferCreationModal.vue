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
                    <StockItemSelector label="Stock Item" v-model="stockItemId" v-bind:required="false" />
                </b-form-group>
                
                <!-- todo datetimeinput -->
                <b-form-group label="Valid Since">
                    <b-form-input type="datetime-local" v-model="validSince" required />
                </b-form-group>

                <b-form-group label="Valid Until">
                    <b-form-input type="datetime-local" v-model="validUntil" />
                </b-form-group>

                <b-button type="submit" @click="createOffer" v-bind:disabled="!canSubmit">Create Offer</b-button>
            </b-form>
        </b-modal>
    </div>
</template>

<script>
import Api from '@/api.js';
import ProductSelector from "@/components/Selection/ProductSelector.vue";
import StockItemSelector from "@/components/Selection/StockItemSelector.vue";

function toDatetimeLocal(date) {    
    var ten = function (i) {
        return (i < 10 ? '0' : '') + i;
    };

    var year = date.getFullYear();
    var month = ten(date.getMonth() + 1);
    var day = ten(date.getDate());

    var hrs = ten(date.getHours());
    var mins = ten(date.getMinutes());
    var secs = ten(date.getSeconds());
    
    return year + '-' + month + '-' + day + 'T' + hrs + ':' + mins + ':' + secs;
}

export default {
    name: 'OfferCreationModal',
    components: {ProductSelector,StockItemSelector},
    props: {
        modalId: String,
        posId: String
    },
    data: function() {
        return {
            productId: "",
            recommendedPrice: "",
            pointOfSaleId: "",
            stockItemId: "",
            validSince: toDatetimeLocal(new Date()),
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
            this.pointOfSaleId = this.posId;
            this.productId = "";
            this.recommendedPrice = "";
            this.stockItemId = "";
            this.validSince = toDatetimeLocal(new Date());
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
                pointOfSaleId: c.pointOfSaleId,
                stockItemId: c.stockItemId,
                validSince: c.validSince,
                validUntil: c.validUntil
            };

            Api.post("offers", formData)
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