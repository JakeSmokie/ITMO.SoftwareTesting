<template>
	<div>
		ABC
	</div>
</template>

<script>
	import EventDetails from './EventDetails';
	import {favoriteEvents} from '../services/favorite-events.service';
	import {eventDetails} from '../services/kudago.service';

	export default {
		name: 'FavoritesEventsPage',
		components: {EventDetails},

		data: () => ({
			favorites: [],
		}),

		async created() {
			const ids = await favoriteEvents();
			const favorites = [];

			for (const id of ids) {
				favorites.push(await eventDetails(id));
			}

			this.favorites = favorites;
		},
	};
</script>

<style scoped>

</style>
