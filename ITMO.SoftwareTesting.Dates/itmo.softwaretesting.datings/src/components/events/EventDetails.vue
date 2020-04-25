<template>
	<div v-if="eventDetails">
		<span style="display: none" class="event-category" v-for="category in eventDetails.categories" :key="category" :text="category"/>
		<span style="display: none" id="event-location" :text="eventDetails.location && eventDetails.location.slug"/>
		<div class="d-flex justify-content-end">
			<b-button-group class="mb-3">
				<b-button
					variant="outline-primary"
					v-on:click="$bvModal.show('create-date-modal')"
					v-if="availableGroups.length > 0"
					id="event-create-date-button"
				>
					Создать встречу
				</b-button>
				<b-button
					:variant="'outline-' + (isFavorite ? 'danger' : 'success')"
					v-on:click="move(!isFavorite)"
					id="event-fav-button"
				>
					{{ isFavorite ? 'Удалить из любимых' : 'Добавить в любимые' }}
				</b-button>
			</b-button-group>
		</div>
		<b-card
			id="event-details-card"
			:title="eventDetails.title"
			:sub-title="eventDetails.tagline"
			:img-src="eventDetails.images[0].image"
			img-alt="Image"
			img-top
		>
			<h6 v-for="(date, i) in eventDetails.dates" :key="i">
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
	</div>
</template>
<script>
	import {mapActions, mapGetters, mapState} from 'vuex';

	export default {
		name: 'event-details',
		computed: {
			...mapState('events', ['favoriteEvents']),
			...mapState('dates', ['dates']),
			...mapGetters(['availableGroups']),
			...mapGetters('events', {
				eventDetails: 'selectedEventDetails',
			}),

			isFavorite() {
				return (this.favoriteEvents || []).includes((this.eventDetails || {id: -1}).id);
			},
		},

		methods: {
			...mapActions('events', ['addFavoriteEvent', 'removeFavoriteEvent']),
			...mapActions('dates', ['createDate']),

			async move(value) {
				if (value) {
					this.addFavoriteEvent(this.eventDetails.id);
				} else {
					this.removeFavoriteEvent(this.eventDetails.id);
				}
			},
		},
	};
</script>
