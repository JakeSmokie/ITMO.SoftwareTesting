<template>
	<div v-if="datesPageLoaded">
		<b-row>
			<b-col>
				<div
					v-for="[groupId, eventIds] in Object.entries(dates)" :key="groupId"
					class="text-left p-4 group-dates-group"
				>
					<h4 class="group-name">{{ (groups.find(x => x.id === +groupId) || {}).name }}</h4>

					<b-list-group>
						<b-list-group-item
							v-for="event in eventIds" :key="event"
							action
							v-on:click="selectEvent(event)"
							:active="selectedEvent === event"
							class="date-event"
						>
							{{ eventsDetails.find(x => x.id === event).title }}
						</b-list-group-item>
					</b-list-group>

				</div>
			</b-col>
			<b-col>
				<event-details/>
			</b-col>
		</b-row>

		<create-date-modal/>
	</div>
	<b-spinner class="m-5" v-else/>
</template>

<script>
	import {mapActions, mapState} from 'vuex';
	import EventDetails from '../components/events/EventDetails';
	import CreateDateModal from '../components/CreateDateModal';

	export default {
		name: 'DatesPage',
		components: {CreateDateModal, EventDetails},
		async created() {
			await this.loadDatesPage();
		},

		computed: {
			...mapState('dates', ['dates']),
			...mapState(['datesPageLoaded']),
			...mapState('groups', ['groups']),
			...mapState('events', ['eventsDetails', 'selectedEvent']),
		},

		methods: {
			...mapActions(['loadDatesPage']),
			...mapActions('events', ['selectEvent']),
		},
	};
</script>

<style scoped>

</style>
