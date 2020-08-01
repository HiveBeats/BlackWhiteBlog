<template>
  <div class="cards" >
    <article v-for="item in items" :key="item.id">
      <FeedCard :style="clrstyles" v-bind:item="item"></FeedCard>
    </article>
  </div>
</template>

<script>
import FeedCard from "./FeedCard";

export default {
  name: "BlogFeed",
  components: {
    FeedCard
  },
  props: {
    count: Number
  },
  data: function () {
    return {
      items: []
    }
  },
  methods: {

  },
  computed: {
    clrstyles() {
      return {
        "border-color": this.bgColor ? "#aaaaaa" : "Black",
        "background": this.bgColor ? "Black" : "White"
      };
    }
  },
  mounted() {
    var idx = 0;
    for (idx = 0; idx < this.count; idx++) {
      const element = {
        title:"Hello World!",
        image:"http://placekitten.com/305/205",
        id: idx
      };
      this.items.push(element);
    }
  }
};
</script>

<style scoped>

.cards {
  max-width: 960px;
  margin: 0 auto 30px;
  
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  grid-gap: 30px;
}

article {
  position: relative;
  border: 3px solid;
}

</style>
