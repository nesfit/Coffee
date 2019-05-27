 /* Based on https://getbootstrap.com/docs/4.1/examples/sign-in/ */

<template>
  <div id="login">
    <b-form @submit="performLogin" class="form-signin">
      <h1 class="h3 mb-3 font-weight-normal">Barista Login</h1>

      <Message v-bind:data="alert" />

      <label for="inputEmail" class="sr-only">Email address</label>
      <input type="email" class="form-control" v-model="emailAddress" placeholder="Email address" required autofocus>

      <label for="inputPassword" class="sr-only">Password</label>
      <input type="password" class="form-control" placeholder="Password" v-model="password" required>

      <button class="btn btn-lg btn-primary btn-block" type="submit">Sign in</button>
    </b-form>
  </div>
</template>

<script>

import Api from '@/api.js'

export default {
  name: "Login",
  data: function() {
      return {
        emailAddress: "",
        password: "",
        alert: {}
      };
  },
  methods: {
      performLogin: function(evt) {
          evt.preventDefault();

          var c = this;

          c.alert = {"info": "Logging in.."};

          Api.post(
              "token/login?saveAsCookie=true",
              { "emailAddress": c.emailAddress, "password": c.password }
            )
            .then(() => {
                c.alert = {};
                c.$emit("logged-in");
            })
            .catch(e => {
                if (e.response && e.response.data.message)
                    c.alert = {"danger": e.response.data.message};
                else
                    c.alert = {"danger":"Login failed."};
            });
      }
  }
}
</script>

<style scoped>
#login {
  display: -ms-flexbox;
  display: flex;
  -ms-flex-align: center;
  align-items: center;
  padding-top: 40px;
  padding-bottom: 40px;
}

.form-signin {
  width: 100%;
  max-width: 330px;
  padding: 15px;
  margin: auto;
}
.form-signin .checkbox {
  font-weight: 400;
}
.form-signin .form-control {
  position: relative;
  box-sizing: border-box;
  height: auto;
  padding: 10px;
  font-size: 16px;
}
.form-signin .form-control:focus {
  z-index: 2;
}
.form-signin input[type="email"] {
  margin-bottom: -1px;
  border-bottom-right-radius: 0;
  border-bottom-left-radius: 0;
}
.form-signin input[type="password"] {
  margin-bottom: 10px;
  border-top-left-radius: 0;
  border-top-right-radius: 0;
}
</style>