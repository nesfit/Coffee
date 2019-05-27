<template>
    <div v-if="posId">
      <b-button variant="success" v-b-modal.newPosAuthUser>Add New</b-button>

      <PagedQuery v-bind:fields="fields" v-bind:endpoint="'pointsOfSale/' + posId + '/authorizedUsers'" @item-clicked="authorizedUserClicked">
            <template slot="userId" slot-scope="data">
              <UserName v-bind:id="data.item.userId" />
            </template>
      </PagedQuery>

      <PosAuthorizedUserCreationModal modalId="newPosAuthUser" :posId="posId" />
      <PosAuthorizedUserDetailsModal modalId="posAuthUserDetails" :posId="posId" v-bind:values="selectedAuthorization" @user-authorized="showDetails" />
    </div>
</template>

<script>
import PagedQuery from '@/components/Query/PagedQuery.vue'
import UserName from '@/components/Display/UserName.vue'
import PosAuthorizedUserCreationModal from '@/components/Modals/PosAuthorizedUserCreationModal.vue'
import PosAuthorizedUserDetailsModal from '@/components/Modals/PosAuthorizedUserDetailsModal.vue'

export default {
  name: 'PointOfSaleAuthorizedUsers',
  components: { PagedQuery, PosAuthorizedUserCreationModal, PosAuthorizedUserDetailsModal,UserName},
  methods: {
    authorizedUserClicked(row) {
      this.showDetails(row.userId);       
    },

    showDetails(userId) {
      var c = this;

      c.$api.get("pointsOfSale/"+this.posId+"/authorizedUsers/"+userId)
        .then(resp => {
          c.selectedAuthorization = resp.data;
          c.$bvModal.show("posAuthUserDetails");
        });
    }
  },
  data: function() {
    return {
      posId: null,
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
    this.posId = this.$route.params.id;
  }
}
</script>