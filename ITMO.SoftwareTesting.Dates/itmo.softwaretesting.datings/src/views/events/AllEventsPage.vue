<template>
	<b-container>
		<b-row class="mb-4">
			<b-col>
				<div class="mb-2">Категория</div>
				<b-form-select
					v-model="selectedCategory"
					:options="categoriesOptions"
					v-on:change="updateEvents"
					id="category-filter"
				/>
			</b-col>
			<b-col>
				<div class="mb-2">Город</div>
				<b-form-select
					v-model="selectedLocation"
					:options="locationsOptions"
					v-on:change="updateEvents"
					id="location-filter"
				/>
			</b-col>
		</b-row>

		<b-row v-if="events.length > 0">
			<b-col>
				<b-list-group>
					<b-list-group-item
						v-for="event in events" :key="event.id"
						href="#"
						class="text-wrap text-left event-list-item"
						v-on:click="selectEvent(event.id)"
						:active="selectedEvent === event.id"
					>
						{{ event.title }}
					</b-list-group-item>
				</b-list-group>
			</b-col>
			<b-col>
				<template v-if="selectedEventDetails">
					<event-details/>
				</template>
				<b-spinner v-else class="mt-5"/>
			</b-col>
		</b-row>
		<b-spinner v-else class="mt-5"/>
	</b-container>
</template>

<script>
	import EventDetails from '../../components/events/EventDetails';
	import {mapActions, mapGetters, mapState} from 'vuex';

	const itemToOption = x => ({
		value: x.slug,
		text: x.name,
	});

	export default {
		name: 'AllEventsPage',
		components: {EventDetails},
		data: () => ({
			selectedCategory: null,
			selectedLocation: null,
		}),

		methods: {
			...mapActions('events', ['loadEvents', 'selectEvent']),

			async updateEvents() {
				await this.loadEvents([this.selectedCategory, this.selectedLocation]);
				await this.selectEvent((this.events[0] || {}).id);
			},
		},

		computed: {
			...mapState('events', ['events', 'selectedEvent']),
			...mapState('kudago', ['locations', 'eventCategories']),
			...mapGetters('events', ['selectedEventDetails']),

			categoriesOptions() {
				return this.eventCategories.map(itemToOption);
			},

			locationsOptions() {
				return this.locations.map(itemToOption);
			},
		},
	};
</script>

