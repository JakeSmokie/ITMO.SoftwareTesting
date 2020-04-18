<template>
  <div class="py-5">
    <router-view></router-view>
  </div>
</template>

<script>
// @ is an alias to /src

import HelloWorld from '../components/HelloWorld';

export default {
  name: 'home',
  components: {
    HelloWorld
  }
}
</script>
