<template>
  <div>
    <img class="feed-card-img" :src="item.image" alt="" />
    <p class="feed-card-title">{{item.title}}</p>
  </div>
</template>

<script>
export default {
  name: "FeedCard",
  props: {
    item: {
      title:String,
      image:String,
      id: Number
    }
  }
};
</script>

<style scoped>

.feed-card-title {
  width: 100%;
  padding: 3px;
}

.fedd-card-img {
  height: 200px;
  width: 100%;
}

</style>