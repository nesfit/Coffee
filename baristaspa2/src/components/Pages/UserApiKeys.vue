<template>
    <div v-if="userId">
      <b-button variant="success" @click="$refs.creationModal.show()">Create New</b-button>
      <UserApiKeyCreationModal ref="creationModal" @api-key-created="$refs.pagedQuery.reload()" v-bind:userId="userId" />

      <PagedQuery ref="pagedQuery" @item-clicked="removeApiKey" v-bind:fields="fields" v-bind:endpoint="'users/' + userId + '/apiKeys'" startingSortBy="label" startingSortDir="asc">
          <template slot="validSince" slot-scope="data">
              <TimeAgo v-bind:moment="data.item.validSince" />
          </template>
      </PagedQuery>
    </div>
</template>

<script>

import PagedQuery from '@/components/Query/PagedQuery.vue'
import TimeAgo from '@/components/Display/TimeAgo.vue'
import UserApiKeyCreationModal from '@/components/Modals/UserApiKeyCreationModal.vue'

export default {
  name: 'UserApiKeys',
  components: { PagedQuery, TimeAgo, UserApiKeyCreationModal },
  mounted() {
    this.userId = this.$route.params.id;
  },
  methods: {
    removeApiKey(item) {
      var c = this;

      this.$bvModal.msgBoxConfirm("Are you sure you want to remove this API key?")
        .then(val => {
          if (!val) return;
          c.$api.delete("users/" + c.userId + "/apiKeys/" + item.id)
            .then(() => c.$refs.pagedQuery.reload())
            .catch(c.$api.showError)
        });
    }
  },
  data: function() {
    return {
      userId: null,
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