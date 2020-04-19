<template>
	<b-container>
		<b-nav tabs align="right" class="w-100 mx-3 mb-3">
			<b-nav-item to="/events">Все</b-nav-item>
			<b-nav-item to="/events/favorites">Любимые</b-nav-item>
		</b-nav>

		<router-view/>
	</b-container>
</template>

<script>
	export default {
		name: 'MainEventsPage',
	};
</script>

<style scoped>

</style>
