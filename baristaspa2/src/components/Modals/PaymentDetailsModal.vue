<template>
    <div>
        <b-modal :id="modalId" title="Payment Details" hide-footer>
            <b-form>
                <b-form-group label="ID" label-for="id">
                    <b-form-input disabled :value="id" />
                </b-form-group>

                <b-form-group label="Created" label-for="created">
                    <b-form-input disabled id="created" v-bind:value="createdText" type="text" />
                </b-form-group>

                <b-form-group label="Amount">
                    <b-form-input v-model="amount"/>
                </b-form-group>          

                <b-form-group label="User">
                    <UserSelector v-model="userId" label="User" v-bind:required="true" />
                </b-form-group>

                <b-form-group label="Source">
                    <b-form-input v-model="source" />
                </b-form-group>

                <b-form-group label="External ID">
                    <b-form-input v-model="externalId" />
                </b-form-group>

                <b-button type="submit" @click="editPayment">Edit Payment</b-button>
                <b-button type="submit" @click="deletePayment" variant="danger">Delete Payment</b-button> 
            </b-form>
        </b-modal>
    </div>
</template>

<script>
import Api from '@/api.js';
import UserSelector from "@/components/Selection/UserSelector.vue";

export default {
    name: 'PaymentDetailsModal',
    components: {UserSelector},
    props: {
        modalId: String,
        values: Object
    },
    computed: {
        createdText: function() {
            if (this.values && this.values.created)
                return new Date(this.values.created).toLocaleString();
            else
                return "";
        }
    },
    data: function() {
        return {
            id: "",
            amount: 0,
            userId: "",
            source: "",
            externalId: ""
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
            this.amount = this.values.amount;
            this.userId = this.values.userId;
            this.source = this.values.source;
            this.externalId = this.values.externalId;
        },
        
        deletePayment: function(evt) {
            evt.preventDefault();
            var c = this;            
            var payment = this.values.id;

            Api.delete("payments/" + payment)
                .then(() => {
                    c.$bvModal.msgBoxOk("Payment deleted");
                    c.$bvModal.hide(c.values.modalId);
                })
                .catch(() => c.$bvModal.msgBoxOk("Payment deletion failed"));
        },

        editPayment: function(evt) {
            evt.preventDefault();
            var c = this;
            var payment = this.values.id;

            Api.put("payments/" + payment, { amount: c.amount, userId: c.userId, source: c.source, externalId: c.externalId })
                .then(() => c.$bvModal.msgBoxOk("Payment updated"))
                .catch(() => c.$bvModal.msgBoxOk("Payment failed to update"));
        }
    }
}
</script>