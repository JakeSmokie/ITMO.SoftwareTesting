<template>
	<b-container>
		<b-row class="mb-4">
			<b-col>
				<div class="mb-2">Категория</div>
				<b-form-select v-model="selectedCategory" :options="categoriesOptions" v-on:change="updateEvents"/>
			</b-col>
			<b-col>
				<div class="mb-2">Город</div>
				<b-form-select v-model="selectedLocation" :options="locationsOptions" v-on:change="updateEvents"/>
			</b-col>
		</b-row>

		<b-row v-if="events.length > 0">
			<b-col>
				<b-list-group>
					<b-list-group-item
						v-for="event in events" :key="event.id"
						href="#"
						class="text-wrap"
						v-on:click="selectEvent(event.id)"
						:active="selectedEvent === event.id"
					>
						{{ event.title }}
					</b-list-group-item>
				</b-list-group>
			</b-col>
			<b-col>
				<template v-if="eventDetails">
					<event-details :event-details="eventDetails" :favorites="favorites" />
<!--					v-on:movedfavorite="" -->
				</template>
				<b-spinner v-else class="mt-5"/>
			</b-col>
		</b-row>
		<b-spinner v-else class="mt-5"/>
	</b-container>
</template>

<script>
	import {eventCategories, eventDetails, events, locations, placeDetails} from '../services/kudago.service';
	import EventDetails from './EventDetails';
	import {favoriteEvents} from '../services/favorite-events.service';

	const itemToOption = x => ({
		value: x.slug,
		text: x.name,
	});

	export default {
		name: 'EventsPage',
		components: {EventDetails},
		data: () => ({
			eventCategories: [],
			locations: [],
			events: [],
			selectedCategory: null,
			selectedLocation: null,
			selectedEvent: null,
			eventDetails: null,
			favorites: [],
		}),

		async created() {
			this.eventCategories = await eventCategories();
			this.locations = await locations();
			this.favorites = await favoriteEvents();

			await this.updateEvents();
		},

		methods: {
			async updateEvents() {
				this.events = await events(this.selectedLocation, this.selectedCategory);
				await this.selectEvent((this.events[0] || {}).id);
			},

			async selectEvent(eventId) {
				this.selectedEvent = eventId;
				this.eventDetails = null;
				this.eventDetails = await eventDetails(eventId);

				const place = this.eventDetails.place || {};
				if (place.id) {
					this.eventDetails.place = await placeDetails(place.id);
				}
			},
		},

		computed: {
			categoriesOptions() {
				return this.eventCategories.map(itemToOption);
			},

			locationsOptions() {
				return this.locations.map(itemToOption);
			},
		},
	};
</script>

