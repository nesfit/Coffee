<template>
    <span>{{ balance }}</span>
</template>

<script>
import Api from '@/api.js'

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

      Api.get("stockOperations/balance/" + c.id)
        .then(bal => c.balance = bal.data)
    }
  },
  mounted() {
    this.reloadBalance();
  }
}
</script>