<template>
  <div>
    <h1>My payments</h1>
    <PagedQuery v-bind:fields="fields" endpoint="payments/me" startingSortBy="created" startingSortDir="desc">
        <template slot="created" slot-scope="data">
            {{ new Date(data.item.created).toLocaleString() }}
        </template>
    </PagedQuery>
  </div>
</template>

<script>

import PagedQuery from "@/components/Query/PagedQuery.vue"

export default {
  name: "MyPayments",
  components: {PagedQuery},
  data: function() {
      return {
          fields: [
            {
                key: "created",
                label: "Created",
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
                sortable: true
            },
            {
                key: "externalId",
                label: "External ID",
                sortable: true
            }
          ]
      }
  }
}
</script>