<template>
    <span>{{ name }}</span>
</template>

<script>
import State from '@/state.js'

export default {
  name: 'PosName',
  data: () => {
      return { name: "Loading.."};
  },
  props: {
    id: String
  },
  watch: {
    id() {
      this.reloadName();
    }
  },
  methods: {
    reloadName() {
      var c = this;
      
      if (!c.id) {
        this.name = "Loading..";
        return;
      }

      State.getPosName(c.$api, c.id)
        .then(n => c.name = n)
        .catch(() => c.name = "Unknown");
    }
  },
  mounted() {
    this.reloadName();
  }
}
</script>