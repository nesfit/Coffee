<template>
    <span>{{ balance }}</span>
</template>

<script>
export default {
  name: 'StockItemBalance',
  data: () => {
      return { balance: "..."};
  },
  props: {
    id: String
  },
  watch: {
    id() {
      this.reloadBalance();
    }
  },
  methods: {
    reloadBalance() {
      var c = this;
      
      if (!c.id) {
        this.balance = "...";
        return;
      }

      c.$api.get("stockOperations/balance/" + c.id)
        .then(bal => c.balance = bal.data)
    }
  },
  mounted() {
    this.reloadBalance();
  }
}
</script>