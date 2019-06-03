<template>
  <div id="app">
    <div id="initializing-container" v-if="loading">
      <Message v-bind:data="{'info':'Initializing..'}" class="initializing" />
    </div>

    <Login v-else-if="shouldShowLoginScreen" v-on:logged-in="hideLoginScreen"></Login>

    <div v-else>
      <b-navbar toggleable="lg" type="dark" variant="dark">
        <b-navbar-brand href="#">Barista</b-navbar-brand>
        <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>

        <b-collapse id="nav-collapse" is-nav>
            <b-navbar-nav class="mr-auto">
              <b-nav-item to="/home" active-class="active">Home</b-nav-item>
              <b-nav-item to="/my-purchases" active-class="active">My Purchases</b-nav-item>
              <b-nav-item to="/my-payments" active-class="active">My Payments</b-nav-item>
              <b-nav-item to="/users" active-class="active" v-if="isAdministrator">Users</b-nav-item>
              <b-nav-item to="/cardAssignment" active-class="active" v-if="isAdministrator">Card Assignment</b-nav-item>
              <b-nav-item to="/products" active-class="active" v-if="isAdvancedUser">Products</b-nav-item>
              <b-nav-item to="/sales" active-class="active" v-if="isAdministrator">Sales</b-nav-item>
              <b-nav-item to="/payments" active-class="active" v-if="isAdministrator">Payments</b-nav-item>
              <b-nav-item to="/accountingGroups" active-class="active" v-if="isAdvancedUser" exact>Accounting Groups</b-nav-item>
              <b-nav-item to="/pointsOfSale" active-class="active" v-if="isAdvancedUser" exact>Points of Sale</b-nav-item>
            </b-navbar-nav>
            
            <b-navbar-nav>
              <b-nav-item to="/passwordChange">Change password</b-nav-item>
              <b-nav-item v-on:click="performLogout">Logout</b-nav-item>
            </b-navbar-nav>
        </b-collapse>
      </b-navbar>

      <b-container id="app-contents-container">
        <router-view></router-view>
      </b-container>
    </div>
  </div>
</template>

<script>
import Login from '@/components/Pages/Login.vue'

export default {
  name: "app",
  components: { Login },
  data: function() {
    return {shouldShowLoginScreen: true, loading: true, policies: {}};
  },
  computed: {
    isAdministrator: function () {
      return this.policies["IsAdministrator"];
    },

    isAdvancedUser: function() {
      return this.policies["IsAdvancedUser"];
    }
  },
  methods: {
    hideLoginScreen: function() {
      this.shouldShowLoginScreen = false;
    },

    performLogout: function() {
      this.$api.post("token/logoutCookie")
        .then(() => this.shouldShowLoginScreen = true);
    },

    loadPolicies: function(policyArray) {
      this.$set(this, "policies", {});
      
      for (var i = 0; i < policyArray.length; i++)
        this.$set(this.policies, policyArray[i], true);
    },

    apiReady: function() {
      var c = this;

      c.$api.showError = function(error, title) {
        var errorMessage = "Error communicating with API: " + error.toString() + ".";

        if (error.response && error.response.data)
          errorMessage += "Code: " + error.response.data + ".";

        if (error.response && error.response.message)
          errorMessage += "Message: " + error.response.message + ".";

        var options = {};
        if (title)
          options.title = title;

        c.$bvModal.msgBoxOk(errorMessage, options);
      };

      c.$api.get("policies/me")
        .then(function(response) {
          c.loadPolicies(response.data);
          c.shouldShowLoginScreen = false;
        })
        .catch(() => c.shouldShowLoginScreen = true)
        .then(() => c.loading = false);
    }
  },
  mounted() {
    var c = this;
    c.apiReady();
  }
}
</script>

<style scoped>
#initializing-container {
  display: -ms-flexbox;
  display: flex;
  -ms-flex-align: center;
  align-items: center;
  padding-top: 40px;
  padding-bottom: 40px;
}

.initializing {
  width: 100%;
  max-width: 330px;
  padding: 15px;
  margin: auto;
}

#app-contents-container {
  padding-top: 40px;
}
</style>