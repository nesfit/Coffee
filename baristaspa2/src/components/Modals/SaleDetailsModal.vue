<template>
    <b-modal :id="modalId" title="Sale Details" hide-footer>
        <b-form>
            <b-form-group label="ID" label-for="id">
                <b-form-input disabled :value="values.id" />
            </b-form-group>

            <b-form-group label="Created" label-for="created">
                <b-form-input disabled id="created" :value="(new Date(values.created).toLocaleString())" type="text" />
            </b-form-group>

            <b-form-group label="Point of sale">
                <PosName :id="values.pointOfSaleId" />
            </b-form-group>

            <b-form-group label="Product">
                <ProductName :id="values.productId" />
            </b-form-group>

            <b-form-group label="Accounting Group">
                <AgName :id="values.accountingGroupId" />
            </b-form-group>

            <b-form-group label="User">
                <UserName :id="values.userId" />
            </b-form-group>

            <b-form-group label="Cost">
                <b-form-input disabled :value="values.cost" />
            </b-form-group>

            <b-form-group label="State">
                <b-form-input disabled v-bind:value="state" />
            </b-form-group>

            <b-form-group label="Reason for cancellation" v-if="state != 'Cancelled'">
                <b-form-input v-model="cancelReason" required />
            </b-form-group>

            <b-button type="submit" v-if="state != 'Cancelled'" variant="warning" @click="cancelSale">Cancel Sale</b-button>
        </b-form>
    </b-modal>
</template>

<script>
import PosName from "@/components/Display/PosName.vue";
import ProductName from "@/components/Display/ProductName.vue";
import AgName from "@/components/Display/AgName.vue";
import UserName from "@/components/Display/UserName.vue";

export default {
    name: 'SaleDetailsForm',
    components: {PosName,ProductName,UserName,AgName},
    props: {
        modalId: String,
        values: Object
    },
    data: function() {
        return {
            state: "",
            cancelReason: ""
        };
    },
    mounted() {
        this.state = this.values.state;
    },
    watch: {
        values() {
            this.state = this.values.state;
        }
    },
    methods: {
        cancelSale: function(evt) {
            evt.preventDefault();
            var c = this;
            
            var saleId = this.values.id;

            c.$api.post("sales/" + saleId + "/cancel", { reason: this.cancelReason })
                .then(() => c.state = "Cancelled")
                .catch(() => c.$bvModal.msgBoxOk("Sale cancellation failed"));
        }
    }
}
</script>