<template>
    <div v-if="posId">
      <b-button variant="success" @click="$refs.creationModal.show()">Create New</b-button>
      <PosApiKeyCreationModal ref="creationModal" @api-key-created="$refs.pagedQuery.reload()" v-bind:posId="posId" />

      <PagedQuery ref="pagedQuery" @item-clicked="removeApiKey" v-bind:fields="fields" v-bind:endpoint="'pointsOfSale/' + posId + '/apiKeys'" startingSortBy="label" startingSortDir="asc">
          <template slot="validSince" slot-scope="data">
              <TimeAgo v-bind:moment="data.item.validSince" />
          </template>
      </PagedQuery>
    </div>
</template>

<script>

import PagedQuery from '@/components/Query/PagedQuery.vue'
import TimeAgo from '@/components/Display/TimeAgo.vue'
import PosApiKeyCreationModal from '@/components/Modals/PosApiKeyCreationModal.vue'

export default {
  name: 'PointOfSaleApiKeys',
  components: { PagedQuery, TimeAgo, PosApiKeyCreationModal },
  mounted() {
    this.posId = this.$route.params.id;
  },
  methods: {
    removeApiKey(item) {
      var c = this;

      this.$bvModal.msgBoxConfirm("Are you sure you want to remove this API key?")
        .then(val => {
          if (!val) return;
          c.$api.delete("pointsOfSale/" + c.posId + "/apiKeys/" + item.id)
            .then(() => c.$refs.pagedQuery.reload())
            .catch(c.$api.showError)
        });
    }
  },
  data: function() {
    return {
      posId: null,
      fields: [
        {
          key: "label",
          label: "Label",
          sortable: true
        },
        {
          key: "validSince",
          label: "Created",
          sortable: true,
        },
        {
          key: "id",
          label: "ID",
          sortable: true
        }
      ]
    };
  }
}
</script>