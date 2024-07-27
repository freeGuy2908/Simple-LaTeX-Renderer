<template>
  <meta name="viewport" content="width=device-width, initial-scale=1.0" />
  <div id="app">
    <LatexConverter />
  </div>
</template>

<script>
import LatexConverter from "./components/LatexConverter.vue";

export default {
  name: "App",
  components: {
    LatexConverter,
  },
};
</script>

<style>
body,
html {
  margin: 0;
  padding: 0;
  width: 100%;
  height: 100%;
}

#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  margin: 0;
  padding: 0;
  width: 100%;
  height: 100%;
}

nav {
  display: flex;
  justify-content: space-around;
  margin-bottom: 1em;
}
</style>
