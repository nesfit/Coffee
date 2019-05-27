<template>
    <div v-if="agId">
      <b-button variant="success" v-b-modal.newAgAuthUser>Add New</b-button>

      <PagedQuery v-bind:fields="fields" v-bind:endpoint="'accountingGroups/' + agId + '/authorizedUsers'" @item-clicked="authorizedUserClicked">
            <template slot="userId" slot-scope="data">
              <UserName v-bind:id="data.item.userId" />
            </template>
      </PagedQuery>

      <AgAuthorizedUserCreationModal modalId="newAgAuthUser" :agId="agId" />
      <AgAuthorizedUserDetailsModal modalId="agAuthUserDetails" :agId="agId" v-bind:values="selectedAuthorization" @user-authorized="showDetails" />
    </div>
</template>

<script>
import Api from '@/api.js'
import PagedQuery from '@/components/Query/PagedQuery.vue'
import UserName from '@/components/Display/UserName.vue'
import AgAuthorizedUserCreationModal from '@/components/Modals/AgAuthorizedUserCreationModal.vue'
import AgAuthorizedUserDetailsModal from '@/components/Modals/AgAuthorizedUserDetailsModal.vue'

export default {
  name: 'AccountingGroupAuthorizedUsers',
  components: { PagedQuery, AgAuthorizedUserCreationModal, AgAuthorizedUserDetailsModal,UserName},
  methods: {
    authorizedUserClicked(row) {
      this.showDetails(row.userId);       
    },

    showDetails(userId) {
      var c = this;

      Api.get("accountingGroups/"+this.agId+"/authorizedUsers/"+userId)
        .then(resp => {
          c.selectedAuthorization = resp.data;
          c.$bvModal.show("agAuthUserDetails");
        });
    }
  },
  data: function() {
    return {
      agId: null,
      selectedAuthorization: {},
      fields: [
        {
          key: "userId",
          label: "User Id",
          sortable: true
        },
        {
          key: "level",
          label: "Level",
          sortable: true
        }
      ]
    };
  },
  mounted() {
    this.agId = this.$route.params.id;
  }
}
</script>