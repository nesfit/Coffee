<template>
    <div>
        <b-modal :id="modalId" title="Create Payment" hide-footer>
            <b-form>
                <b-form-group label="User">
                    <UserSelector v-model="userId" label="User" v-bind:required="true" />
                </b-form-group>

                <b-form-group label="Amount">
                    <b-form-input v-model="amount"/>
                </b-form-group>          

                <b-form-group label="Source">
                    <b-form-input v-model="source" />
                </b-form-group>

                <b-form-group label="External ID">
                    <b-form-input v-model="externalId" />
                </b-form-group>

                <b-button type="submit" @click="createPayment" v-bind:disabled="!userId">Create Payment</b-button>
            </b-form>
        </b-modal>
    </div>
</template>

<script>
import UserSelector from "@/components/Selection/UserSelector.vue";

export default {
    name: 'PaymentCreationModal',
    components: {UserSelector},
    props: {
        modalId: String
    },
    data: function() {
        return {
            amount: 0,
            userId: "",
            source: "",
            externalId: ""
        };
    },
    methods: {
        createPayment: function(evt) {
            evt.preventDefault();
            var c = this;

            c.$api.post("payments", { amount: c.amount, userId: c.userId, source: c.source, externalId: c.externalId })
                .then(resp => {
                    c.$emit("payment-created", resp.data.id);
                    c.$bvModal.hide(c.modalId);
                })
                .catch(c.$api.showError);
        }
    }
}
</script>