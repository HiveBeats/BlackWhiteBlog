<template>
  <div>
    <img class="top-card-img" :src="item.image" alt="" />
    <p class="top-card-title">{{item.title}}</p>
  </div>
</template>

<script>
export default {
  name: "TopCard",
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

.top-card-title {
  padding: 3px;
}

.top-card-img {
  width: 100px;
}

</style>