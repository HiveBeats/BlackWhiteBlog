<template>
  <div id="app" class="body-container">
    <div class="header">
      <img width="5%" src="./assets/logo.png">
    </div>
    <div class="sidebar">
      <TopNews></TopNews>
    </div>
    <div class="main-content">
     <BlogFeed v-bind:count="6"></BlogFeed>
    </div>
    <div class="aside-bar"></div>
    <footer>
      <img width="25%" src="./assets/logo.png">
    </footer>
  </div>
</template>

<script>
import BlogFeed from "./components/BlogFeed";
import TopNews from "./components/TopNews";
export default {
  name: "App",
  components: {
    BlogFeed,
    TopNews
  }
};
</script>

<style>
#app {
  font-family: "Avenir", Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

.body-container {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}

@media (min-width: 768px) {
  .body-container {
    display: grid;
    grid-template-columns: 200px 1fr 200px;
    grid-template-rows: auto 1fr auto;
  }
}

.header {
  grid-column: span 3;
  padding: 30px;
  text-align: center;
  font-size: 1.4em;
  background-color: #369;
  color: white;
}

.main-content {
  flex: 1;
  padding: 20px;
}

.sidebar {
  background-color: #f90;
  padding: 20px;
}

.aside-bar {
  padding: 20px;
  background-color: #936;
}

footer {
  grid-column: span 3;
  padding: 30px;
  text-align: center;
  font-size: 1.4em;
  background-color: #690;
  color: white;
}

/* RESET.CSS */

html, body, div, span, applet, object, iframe,
h1, h2, h3, h4, h5, h6, p, blockquote, pre,
a, abbr, acronym, address, big, cite, code,
del, dfn, em, img, ins, kbd, q, s, samp,
small, strike, strong, sub, sup, tt, var,
b, u, i, center,
dl, dt, dd, ol, ul, li,
fieldset, form, label, legend,
table, caption, tbody, tfoot, thead, tr, th, td,
article, aside, canvas, details, embed, 
figure, figcaption, footer, header, hgroup, 
menu, nav, output, ruby, section, summary,
time, mark, audio, video {
	margin: 0;
	padding: 0;
	border: 0;
	font-size: 100%;
	font: inherit;
	vertical-align: baseline;
}
/* HTML5 display-role reset for older browsers */
article, aside, details, figcaption, figure, 
footer, header, hgroup, menu, nav, section {
	display: block;
}
body {
	line-height: 1;
}
ol, ul {
	list-style: none;
}
blockquote, q {
	quotes: none;
}
blockquote:before, blockquote:after,
q:before, q:after {
	content: '';
	content: none;
}
table {
	border-collapse: collapse;
	border-spacing: 0;
}

/* apply a natural box layout model to all elements, but allowing components to change */
html {
  box-sizing: border-box;
}
*, *:before, *:after {
  box-sizing: inherit;
}
/* **** */
</style>
