<template>
	<b-container>
		<div v-if="favoriteEvents.length === 0">
			Здесь пока пусто
		</div>
		<b-row v-if="favoriteEventsDetails.length > 0">
			<b-col>
				<b-list-group>
					<b-list-group-item
						v-for="event in favoriteEventsDetails" :key="event.id"
						href="#"
						class="text-wrap"
						v-on:click="setSelectedEvent(event.id)"
						:active="selectedEvent === event.id"
					>
						{{ event.title }}
					</b-list-group-item>
				</b-list-group>
			</b-col>
			<b-col>
				<template v-if="selectedEvent">
					<event-details :event-details="selectedEventDetails"/>
				</template>
			</b-col>
		</b-row>
		<b-spinner v-else class="mt-5"/>
	</b-container>
</template>

<script>
	import {mapActions, mapGetters, mapMutations, mapState} from 'vuex';
	import EventDetails from '../../components/EventDetails';

	export default {
		name: 'FavoritesEventsPage',
		components: {EventDetails},

		async created() {
			await this.loadFavoriteEventsDetails();
			this.selectedEvent = this.favoriteEvents[0];
		},

		methods: {
			...mapActions('events/favorites', ['loadFavoriteEventsDetails']),
			...mapMutations('events/favorites', ['setSelectedEvent']),
		},

		computed: {
			...mapState('events/favorites', ['favoriteEventsDetails', 'favoriteEvents', 'selectedEvent']),
			...mapGetters('events/favorites', ['selectedEventDetails']),
		},
	};
</script>

<style scoped>

</style>
