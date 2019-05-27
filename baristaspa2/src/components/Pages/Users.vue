<template>
    <div>
      <h1>Users</h1>

      <b-button variant="success" @click="$refs.creationModal.show()">Create New</b-button>
      <UserCreationModal ref="creationModal" @user-created="gotoUser" />
   
      <PagedQuery v-bind:fields="fields" endpoint="users/details" startingSortBy="fullName" startingSortDir="desc"
        @item-clicked="showUserDetails">
      </PagedQuery>
  </div>
</template>

<script>
import PagedQuery from '@/components/Query/PagedQuery.vue'
import UserCreationModal from '@/components/Modals/UserCreationModal.vue'

export default {
  name: 'Users',
  components: { PagedQuery, UserCreationModal },
  data: function() {
    return {
      fields: [
        {
            key: "fullName",
            label: "Full Name",
            sortable: true
        },
        {
            key: "emailAddress",
            label: "E-mail address",
            sortable: true
        },
        {
            key: "isAdministrator",
            label: "Is Administrator?",
            sortable: true
        },
        {
            key: "isActive",
            label: "Is Active?",
            sortable: true
        }
      ]
    };
  },
  methods: {
    showUserDetails(data) {
      this.gotoUser(data.id);
    },

    gotoUser(id) {
      this.$router.push("/user/" + id);
    }
  }
}
</script>