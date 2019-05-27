<template>
  <div>
    <h1>Password change</h1>

    <b-alert v-bind:show="shouldShowAlert" v-bind:variant="alertVariant">{{ alertText }}</b-alert>

    <b-form @submit="performPasswordChange">
        <b-form-group label="Current password:">
            <b-form-input v-model="oldPassword" type="password" required></b-form-input>
        </b-form-group>

        <b-form-group label="New password:">
            <b-form-input v-model="newPassword" type="password" required></b-form-input>
        </b-form-group>

        <b-form-group label="New password (again):">
            <b-form-input v-model="newPasswordAgain" type="password" required></b-form-input>
        </b-form-group>

        <b-button type="submit" variant="primary">Change password</b-button>
    </b-form>
  </div>
</template>

<script>
export default {
  name: "PasswordChange",
  data: function() {
      return {
        oldPassword: "",
        newPassword: "",
        newPasswordAgain: "",
        alertVariant: "",
        alertText: null
      };
  },
  computed: {
      shouldShowAlert: function() {
          return this.alertText !== null;
      }
  },
  methods: {
      setAlert: function(variant, text) {
          if (variant === null) {
              this.alertText = this.alertVariant = null;
              return;
          }

          this.alertText = text;
          this.alertVariant = variant;
      },
      performPasswordChange: function(evt) {
          var c = this;
          evt.preventDefault();

          if (this.newPassword != this.newPasswordAgain) {
              this.setAlert("warning", "New passwords do not match, please try again.");
              return;
          }

          c.$api.post(
              "users/me/changePassword",
              { newPassword: c.newPassword, newPasswordAgain: c.newPasswordAgain, oldPassword: c.oldPassword }
            )
            .then(() => c.setAlert("success", "Password changed successfully, remember to use it next time you log in."))
            .catch(error => {
                if (error.response && error.response.data && error.response.data.message)
                    c.setAlert("danger", error.response.data.message);
                else
                    c.setAlert("danger", "Could not change password");
            });
      }
  }
}
</script>

<style scoped>
</style>