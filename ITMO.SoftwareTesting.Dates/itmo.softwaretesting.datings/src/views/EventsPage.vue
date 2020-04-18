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
				<b-card
					v-if="eventDetails"
					:title="eventDetails.title"
					:sub-title="eventDetails.tagline"
					:img-src="eventDetails.images[0].image"
					img-alt="Image"
					img-top
				>
					<h6 v-for="date in eventDetails.dates">
						{{ new Date(date.start * 1000 + 10800000).toLocaleString('ru-RU') }}
						–
						{{ new Date(date.end * 1000 + 10800000).toLocaleString('ru-RU') }}
					</h6>

					<template v-if="eventDetails.place">
						<div v-if="eventDetails.place.title" class="py-2">
							<span>{{ eventDetails.place.title }}</span> <br>
							<span>{{ eventDetails.place.address }}, {{ eventDetails.place.subway }}</span>
						</div>
						<b-spinner variant="success" small v-else/>
					</template>

					<b-card-text class="text-justify" v-html="eventDetails.description"/>
					<b-card-text class="text-justify" v-html="eventDetails.body_text"/>
				</b-card>
			</b-col>
		</b-row>
	</b-container>
</template>

<script>
	import {eventCategories, eventDetails, events, locations, placeDetails} from '../services/kudago.service';

	const itemToOption = x => ({
		value: x.slug,
		text: x.name,
	});

	export default {
		name: 'EventsPage',

		data: () => ({
			eventCategories: [],
			locations: [],
			events: [],
			selectedCategory: null,
			selectedLocation: null,
			selectedEvent: null,
			eventDetails: null,
		}),

		async created() {
			this.eventCategories = await eventCategories();
			this.locations = await locations();

			await this.updateEvents();
		},

		methods: {
			async updateEvents() {
				this.events = await events(this.selectedLocation, this.selectedCategory);
			},

			async selectEvent(eventId) {
				this.selectedEvent = eventId;
				this.eventDetails = await eventDetails(eventId);

				if (this.eventDetails.place.id) {
					this.eventDetails.place = await placeDetails(this.eventDetails.place.id);
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

<style scoped>

</style>
