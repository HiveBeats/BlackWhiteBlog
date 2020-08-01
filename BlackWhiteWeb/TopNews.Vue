<template>
  <div class="cards" >
    <article v-for="item in items" :key="item.id">
      <TopCard v-bind:item="item"></TopCard>
    </article>
  </div>
</template>

<script>
import TopCard from "./TopCard";

export default {
  name: "TopNews",
  components: {
    TopCard
  },
  data: function () {
    return {
      items: []
    }
  },
  methods: {

  },
  mounted() {
    var idx = 0;
    for (idx = 0; idx < 3; idx++) {
      const element = {
        title:"Hello World! {{idx}} times",
        image:"http://placekitten.com/305/205",
        id: idx
      };
      this.items.push(element);
    }
  }
};
</script>

<style scoped>


</style>