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
	import {mapActions} from 'vuex';

	export default {
		name: 'MainEventsPage',

		async created() {
			await this.loadFavoriteEvents();
		},

		methods: {
			...mapActions('events/favorites', ['loadFavoriteEvents']),
		}
	};
</script>

<style scoped>

</style>
