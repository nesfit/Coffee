<template>
    <span v-b-tooltip.hover v-bind:title="this.moment">{{ timeAgo }}</span>
</template>

<script>

export default {
  name: 'TimeAgo',
  data() {
      return { timeout: null, now: new Date() };
  },
  props: {
    moment: String
  },
  computed: {
    diff() {
      return ((new Date(this.moment)) - (this.now)) / 1000;
    },
    timeAgo() {
      var diff = Math.floor(this.diff);
      var diffAbs = Math.abs(diff);
      var saying = "";

      if (diffAbs < 1)
        return "now";

      var mins;

      if (diffAbs < 60)
        saying = diffAbs + " second" + (diffAbs > 1 ? "s" : "");
      else if (diffAbs < 3600) {
        mins = Math.floor(diffAbs / 60);
        saying = mins + " minute" + (mins > 1 ? "s" : "");
      }
      else if (diffAbs < 3600*24) {
        var hrs = Math.floor(diffAbs / 3600);
        mins = Math.floor((diffAbs - hrs*3600) / 60);

        saying = hrs + " hour" + (hrs > 1 ? "s" : "") + " " + mins + " minute" + (mins > 1 ? "s" : "");
      }
      else if (diffAbs < 3600*24*7) {
        var days = Math.floor(diffAbs / (3600*24));
        saying = days + " day" + (days > 1 ? "s" : "");
      }
      else
        return new Date(this.moment).toLocaleString();

      if (diff < 0)
        return saying + " ago";
      else
        return "in " + saying;
    }
  },
  watch: {
    moment() {
      this.reloadMoment();
    }
  },
  methods: {
    reloadMoment() {
      this.now = new Date();
      this.renewInterval();
    },

    renewInterval() {
      if (this.timeout) {
        clearTimeout(this.timeout);
        this.timeout = null;
      }

      var waitDuration = 1000;
      var diffAbs = Math.abs(this.diff);

      if (diffAbs > 3600)
        waitDuration = 3600000;
      else if (diffAbs > 60)
        waitDuration = 60000;
      
      this.timeout = setTimeout(() => this.reloadMoment(), waitDuration);
    }
  },
  mounted() {
    this.reloadMoment();
  }
}
</script>