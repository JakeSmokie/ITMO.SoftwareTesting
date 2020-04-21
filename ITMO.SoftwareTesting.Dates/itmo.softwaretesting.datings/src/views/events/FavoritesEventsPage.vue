<template>
	<b-container>
		<template v-if="favoriteEventsLoaded">
			<div v-if="favoriteEvents.length === 0">
				Здесь пока пусто
			</div>
			<template v-else>
				<b-row v-if="favoriteEventsDetails.length > 0">
					<b-col>
						<b-list-group>
							<b-list-group-item
								v-for="event in favoriteEventsDetails" :key="event.id"
								href="#"
								class="text-wrap text-left"
								v-on:click="selectEvent(event.id)"
								:active="selectedEvent === event.id"
							>
								{{ event.title }}
							</b-list-group-item>
						</b-list-group>
					</b-col>
					<b-col>
						<template v-if="selectedEvent">
							<event-details/>
						</template>
					</b-col>
				</b-row>
			</template>
		</template>
		<b-spinner v-else class="mt-5"/>
	</b-container>
</template>

<script>
	import {mapActions, mapGetters, mapMutations, mapState} from 'vuex';
	import EventDetails from '../../components/events/EventDetails';

	export default {
		name: 'FavoritesEventsPage',
		components: {EventDetails},

		methods: {
			...mapMutations('events', ['selectEvent']),
		},

		computed: {
			...mapState('events', ['favoriteEvents', 'favoriteEventsLoaded']),
			...mapState('events', ['events', 'selectedEvent']),
			...mapGetters('events', ['favoriteEventsDetails']),
		},
	};
</script>

<style scoped>

</style>
