<template>
    <div>
      <h1 v-if="!hideHeading">Payments</h1>

      <b-button variant="success" v-b-modal.createPayment>Create New</b-button>
    
      <PagedQuery v-bind:fields="fields" endpoint="payments" startingSortBy="created" startingSortDir="desc" @item-clicked="paymentClicked">
        <template slot="created" slot-scope="data">
          {{ new Date(data.item.created).toLocaleString() }}
        </template>

        <template slot="userId" slot-scope="data">
          <UserName v-bind:id="data.item.userId" />
        </template>
      </PagedQuery>

      <PaymentDetailsModal modalId="paymentDetails" v-bind:values="selectedPayment" />
      <PaymentCreationModal modalId="createPayment" @payment-created="paymentCreated" />
  </div>
</template>

<script>
import Api from '@/api.js'
import PagedQuery from '@/components/Query/PagedQuery.vue'
import UserName from '@/components/Display/UserName.vue'
import PaymentDetailsModal from "@/components/Modals/PaymentDetailsModal.vue"
import PaymentCreationModal from "@/components/Modals/PaymentCreationModal.vue"

export default {
  name: 'Payments',
  components: { PaymentDetailsModal, PaymentCreationModal, PagedQuery, UserName, },
  props: {
    hideHeading: Boolean
  },
  methods: {
    paymentClicked(row) {
      var paymentId = row.id;
      this.showPaymentDetails(paymentId);
    },

    showPaymentDetails(paymentId) {
      var c = this;

      Api.get("payments/" + paymentId)
        .then(resp => {
          c.selectedPayment = resp.data;
          c.$bvModal.show("paymentDetails");
        });
    },

    paymentCreated(paymentId) {
      this.showPaymentDetails(paymentId);
    }
  },
  data: function() {
    return {
      selectedPayment: {},
      fields: [
        {
          key: "created",
          label: "Created",
          sortable: true
        },
        {
          key: "userId",
          label: "User",
          sortable: true
        },
        {
          key: "amount",
          label: "Amount",
          sortable: true
        },
        {
          key: "source",
          label: "Source",
          sortable: true,
        },
        {
          key: "externalId",
          label: "External ID",
          sortable: true
        }
      ]
    };
  }  
}
</script>

<style>
</style>