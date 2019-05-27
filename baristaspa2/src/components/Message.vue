<template>
    <b-alert v-bind:show="shouldShowAlert" v-bind:variant="alertVariant">{{alertText}}</b-alert>
</template>

<script>
export default {
  name: 'Message',
  props: {
    data: Object
  },
  computed: {
      shouldShowAlert: function() {
          return typeof(this.data) !== typeof(undefined) && Object.keys(this.data).length == 1 ? true : false;
      },

      alertVariant: function() {
          return typeof(this.data) !== typeof(undefined) && Object.keys(this.data).length == 1  ? Object.keys(this.data)[0] : "danger";
      },

      alertText: function() {
          return typeof(this.data) !== typeof(undefined) && Object.keys(this.data).length == 1  ? this.data[Object.keys(this.data)[0]] : "";
      }
  }
}
</script>